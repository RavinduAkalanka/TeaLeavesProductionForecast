import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-hero-section',
  imports: [RouterModule],
  templateUrl: './hero-section.component.html',
  styleUrl: './hero-section.component.css'
})
export class HeroSectionComponent {
  
  onImageError(event: any) {
    console.log('Image failed to load:', event);
    // Hide the image and show fallback
    event.target.style.display = 'none';
  }
  
  onImageLoad(event: any) {
    console.log('Image loaded successfully:', event);
    // Hide the fallback when image loads
    const fallback = event.target.parentElement.querySelector('.image-fallback');
    if (fallback) {
      fallback.style.display = 'none';
    }
  }
}
