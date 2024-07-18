import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTaskModelComponent } from './add-task-model.component';

describe('AddTaskModelComponent', () => {
  let component: AddTaskModelComponent;
  let fixture: ComponentFixture<AddTaskModelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTaskModelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddTaskModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
