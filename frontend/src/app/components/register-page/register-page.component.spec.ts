import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterPageComponent } from './register-page.component';

describe('RegisterPageComponent', () => {
  let component: RegisterPageComponent;
  let fixture: ComponentFixture<RegisterPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterPageComponent]
    }).compileComponents();
    fixture = TestBed.createComponent(RegisterPageComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  it('should have registerForm property', () => {
    expect(component.hasOwnProperty('registerForm')).toBeTrue();
  });

  it('should have onSubmit method', () => {
    expect(typeof component.onSubmit).toBe('function');
  });
});
