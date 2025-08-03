import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToasterMessage {
  id: string;
  message: string;
  type: 'success' | 'error';
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  private messagesSubject = new BehaviorSubject<ToasterMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();

  constructor() { }

  private generateId(): string {
    return Math.random().toString(36).substr(2, 9);
  }

  success(message: string, duration: number = 3000): void {
    this.showMessage(message, 'success', duration);
  }

  error(message: string, duration: number = 5000): void {
    this.showMessage(message, 'error', duration);
  }

  private showMessage(message: string, type: 'success' | 'error', duration: number): void {
    const id = this.generateId();
    const toasterMessage: ToasterMessage = { id, message, type, duration };
    
    const currentMessages = this.messagesSubject.value;
    this.messagesSubject.next([...currentMessages, toasterMessage]);

    // Auto remove message after duration
    setTimeout(() => {
      this.removeMessage(id);
    }, duration);
  }

  removeMessage(id: string): void {
    const currentMessages = this.messagesSubject.value;
    const filteredMessages = currentMessages.filter(msg => msg.id !== id);
    this.messagesSubject.next(filteredMessages);
  }
}
