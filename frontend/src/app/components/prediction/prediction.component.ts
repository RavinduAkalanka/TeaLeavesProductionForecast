import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ToasterService } from '../../services/toaster.service';
import { PredictionService } from '../../services/prediction.service';
import { Prediction } from '../../model/Prediction';

@Component({
  selector: 'app-prediction',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './prediction.component.html',
  styleUrl: './prediction.component.css',
})
export class PredictionComponent implements OnInit {
  predictionForm!: FormGroup;
  predictionResult: number | null = null;

  years: number[] = [
    new Date().getFullYear(),
    new Date().getFullYear() + 1,
    new Date().getFullYear() + 2,
    new Date().getFullYear() + 3,
    new Date().getFullYear() + 4,
    new Date().getFullYear() + 5,
    new Date().getFullYear() + 6,
    new Date().getFullYear() + 7,
    new Date().getFullYear() + 8,
    new Date().getFullYear() + 9,
  ];

  months: string[] = [
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

  constructor(
    private fb: FormBuilder,
    private toasterService: ToasterService,
    private predictionService: PredictionService
  ) {}

  ngOnInit(): void {
    this.predictionForm = this.fb.group({
      Year: ['', Validators.required],
      Month: ['', Validators.required],
      PlantCount: ['', [Validators.required, Validators.min(1)]],
      FertilizerType: ['', Validators.required],
      Pruning: ['', Validators.required],
      SoilPH: [
        '',
        [Validators.required, Validators.min(0), Validators.max(14)],
      ],
      AvgRainfall: ['', [Validators.required, Validators.min(0)]],
      AverageTemperature: ['', Validators.required],
      AvgHumidityPercent: ['', [Validators.required]],
    });
  }

  resetForm(): void {
    this.predictionForm.reset();
  }

  onSubmit(): void {
    const userId = sessionStorage.getItem('userId');
    const formData: Prediction = {
      ...this.predictionForm.value,
      userId: Number(userId),
    };

    this.predictionService.prediction(formData).subscribe({
      next: (res: any) => {
        console.log('Prediction Result: ', res);
        this.predictionResult = res.predictionResult;
      },
      error: (err: any) => {
        const backendError =
          err.error?.error || 'Prediction failed. Please try again.';
        console.log('Error:', backendError);
        this.toasterService.error(backendError);
      },
    });
  }
}
