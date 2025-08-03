import { Component } from '@angular/core';
import { FooterComponent } from '../footer/footer.component';
import { HeroSectionComponent } from '../hero-section/hero-section.component';
import { AboutComponent } from '../about/about.component';
import { ContactComponent } from '../contact/contact.component';
import { HowToWorkComponent } from "../how-to-work/how-to-work.component";

@Component({
  selector: 'app-landing-page',
  imports: [FooterComponent, HeroSectionComponent, AboutComponent, ContactComponent, HowToWorkComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

}
