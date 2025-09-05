import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PredictionComponent } from './prediction.component';

describe('PredictionComponent', () => {
  let component: PredictionComponent;
  let fixture: ComponentFixture<PredictionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PredictionComponent]
    }).compileComponents();
    fixture = TestBed.createComponent(PredictionComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should have predictionForm property', () => {
    expect(component.hasOwnProperty('predictionForm')).toBeTrue();
  });

  it('should have onSubmit method', () => {
    expect(typeof component.onSubmit).toBe('function');
  });
});
