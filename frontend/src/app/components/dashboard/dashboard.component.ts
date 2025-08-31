import { AfterViewInit, Component, OnInit } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { GetAllPrediction } from '../../model/Prediction';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import autoTable from 'jspdf-autotable';
import { Chart, registerables } from 'chart.js';
import { User } from '../../model/UserRegister';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit, AfterViewInit {
  PredictionList: GetAllPrediction[] = [];
  PagePredictionLists: GetAllPrediction[] = [];
  FilteredList: GetAllPrediction[] = [];
  UserDetails: any;
  filterFrom: string = '';
  filterTo: string = '';

  // Pagination settings
  pageSize: number = 5;
  currentPage: number = 1;
  totalPages: number = 0;

  currentMonthName: string = '';
  totalPredictions: number = 0;
  avgPredictedYield: number = 0;
  highestPredictedYield: number = 0;
  lowestPredictedYield: number = 0;

  lineChart: any;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.getAllPredictions();
    this.getUserDetails();
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {}

  private getAllPredictions(): void {
    const userId = sessionStorage.getItem('userId');

    if (!userId) {
      console.error('User ID not found in sessionStorage');
      return;
    }

    this.dashboardService.getAllPredictionById(+userId).subscribe({
      next: (res) => {
        console.log('Predictions fetched successfully:', res);
        this.PredictionList = res.sort(
          (a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        );
        this.calculateCurrentMonthSummary();
        console.log('Prediction List:', this.PredictionList);

        // Initially no filter applied
        this.FilteredList = [...this.PredictionList];

        this.totalPages = Math.ceil(this.PredictionList.length / this.pageSize);
        this.updatePage();
        this.renderLineChart();
      },
      error: (error) => {
        console.error('Error fetching predictions:', error);
      },
    });
  }

  applyDateFilter(): void {
    let filtered = this.PredictionList;

    if (this.filterFrom) {
      const fromDate = new Date(this.filterFrom);
      filtered = filtered.filter((p) => new Date(p.createdAt) >= fromDate);
    }

    if (this.filterTo) {
      const toDate = new Date(this.filterTo);
      filtered = filtered.filter((p) => new Date(p.createdAt) <= toDate);
    }

    this.FilteredList = filtered; // save filtered data for PDF

    // Update pagination after filtering
    this.totalPages = Math.ceil(filtered.length / this.pageSize);
    this.currentPage = 1;
    this.updatePage(filtered);
  }

  clearDateFilter(): void {
    this.filterFrom = '';
    this.filterTo = '';

    this.totalPages = Math.ceil(this.PredictionList.length / this.pageSize);
    this.currentPage = 1;
    this.updatePage();
  }

  updatePage(filteredList?: GetAllPrediction[]): void {
    const list = filteredList || this.PredictionList;
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.PagePredictionLists = list.slice(startIndex, endIndex);
    console.log('Pagination:', this.PagePredictionLists);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePage();
    }
  }

  exportToPDF(): void {
    if (!this.FilteredList || this.FilteredList.length === 0) {
      alert('No data to export');
      return;
    }

    const doc = new jsPDF();

    const columns = [
      'Date',
      'User/Farm',
      'Plant Count',
      'Fertilizer',
      'Pruning',
      'Soil pH',
      'Avg Rainfall',
      'Avg Temp',
      'Avg Humidity',
      'Predicted Yield',
    ];

    const rows = this.FilteredList.map((p) => [
      new Date(p.createdAt).toLocaleDateString(),
      p.userId,
      p.plantCount,
      p.fertilizerType,
      p.pruning,
      p.soilPH,
      p.avgRainfall,
      p.averageTemperature,
      p.avgHumidityPercent,
      p.predictionResult.toFixed(2),
    ]);

    autoTable(doc, {
      head: [columns],
      body: rows,
      startY: 20,
      styles: { fontSize: 8 },
      headStyles: { fillColor: [34, 177, 76] },
    });

    doc.save('Predictions.pdf');
  }

  //Calculate Summary card for current months
  calculateCurrentMonthSummary(): void {
    const now = new Date();
    const currentMonth = now.getMonth();
    const currentYear = now.getFullYear();

    const monthNames = [
      'January',
      'February',
      'March',
      'April',
      'May',
      'June',
      'July',
      'August',
      'September',
      'October',
      'November',
      'December',
    ];
    this.currentMonthName = monthNames[currentMonth];

    const monthPredictions = this.PredictionList.filter((p) => {
      const date = new Date(p.createdAt);
      return (
        date.getMonth() === currentMonth && date.getFullYear() === currentYear
      );
    });

    this.totalPredictions = monthPredictions.length;
    if (monthPredictions.length > 0) {
      const yields = monthPredictions.map((p) => p.predictionResult);
      this.avgPredictedYield =
        yields.reduce((a, b) => a + b, 0) / yields.length;
      this.highestPredictedYield = Math.max(...yields);
      this.lowestPredictedYield = Math.min(...yields);
    } else {
      this.avgPredictedYield = 0;
      this.highestPredictedYield = 0;
      this.lowestPredictedYield = 0;
    }
  }

  renderLineChart(): void {
    const ctx = (
      document.getElementById('yieldLineChart') as HTMLCanvasElement
    )?.getContext('2d');
    if (!ctx) return;

    const monthlyMap: { [month: number]: { sum: number; count: number } } = {};

    // Initialize all months with 0 values
    for (let m = 0; m < 12; m++) {
      monthlyMap[m] = { sum: 0, count: 0 };
    }

    // Aggregate predictions by month
    this.PredictionList.forEach((p) => {
      const date = new Date(p.createdAt);
      const month = date.getMonth(); // 0-11
      monthlyMap[month].sum += p.predictionResult;
      monthlyMap[month].count += 1;
    });

    const monthNames = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];

    // Labels = month names
    const labels = monthNames;

    // Data = average of predictions per month
    const data = Object.values(monthlyMap).map((m) =>
      m.count > 0 ? m.sum / m.count : 0
    );

    // Destroy previous chart if exists
    if (this.lineChart) this.lineChart.destroy();

    this.lineChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels,
        datasets: [
          {
            label: 'Average Predicted Yield (kg)',
            data,
            borderColor: 'rgba(34, 177, 76, 1)',
            backgroundColor: 'rgba(34, 177, 76, 0.2)',
            tension: 0.3,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: true, position: 'top' },
        },
        scales: {
          x: { title: { display: true, text: 'Month' } },
          y: {
            title: { display: true, text: 'Average Predicted Yield (kg)' },
            beginAtZero: true,
          },
        },
      },
    });
  }

  getUserDetails(): void {
    const userId = sessionStorage.getItem('userId');
    if (!userId) {
      console.error('User ID not found in sessionStorage');
      return;
    }
    this.dashboardService.getUserById(+userId).subscribe({
      next: (res: User) => {
        this.UserDetails = res;
        console.log('User Details:', this.UserDetails);
      },
      error: (err) => {
        console.error('Failed to load user:', err);
      },
    });
  }
}
