import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToasterService, ToasterMessage } from '../../services/toaster.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-toaster',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.css']
})
export class ToasterComponent implements OnInit, OnDestroy {
  messages: ToasterMessage[] = [];
  private subscription: Subscription = new Subscription();

  constructor(private toasterService: ToasterService) {}

  ngOnInit(): void {
    this.subscription = this.toasterService.messages$.subscribe(
      messages => {
        this.messages = messages;
      }
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  removeMessage(id: string): void {
    this.toasterService.removeMessage(id);
  }
}
