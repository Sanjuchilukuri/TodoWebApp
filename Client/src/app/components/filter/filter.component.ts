import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { filters } from '../../constants/constants';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [CommonModule, NgFor, FormsModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.scss'
})
export class FilterComponent {

  @Output() filterChange:EventEmitter<{ status: string, date: Date }>;
  todayDate: string;
  selectedStatus: string;
  filters;
  filterOptions: { status: string, date: Date };
  maxDate: string;
  
  constructor() {
    this.todayDate = new Date().toISOString().split('T')[0];
    this.filters = filters;
    this.filterOptions = { status: filters.All.value, date: new Date(this.todayDate) };
    this.filterChange = new EventEmitter<{ status: string, date: Date }>();
    this.selectedStatus = filters.All.value;
    this.maxDate = new Date().toISOString().split('T')[0];
  }

  ChangeDateHandler(event: Event) {
    this.filterOptions.date = new Date((event.target as HTMLInputElement).value);
  }

  ChangeStatusHandler(event: Event) {
    this.filterOptions.status = (event.target as HTMLInputElement).value;
  }

  applyFilters() {
    this.filterChange.emit(this.filterOptions);
  }

  resetFilters() {
    this.todayDate = new Date().toISOString().split('T')[0];
    this.selectedStatus = filters.All.value;
    this.filterOptions = { status: this.selectedStatus, date: new Date(this.todayDate)};
    this.filterChange.emit(this.filterOptions);
  }

}
