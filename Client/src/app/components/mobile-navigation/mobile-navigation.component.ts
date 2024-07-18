import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-mobile-navigation',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mobile-navigation.component.html',
  styleUrl: './mobile-navigation.component.scss',
})
export class MobileNavigationComponent implements OnInit{
  @ViewChild('options') myOptionRef!: ElementRef;
  selectedItem: string = 'Dashboard';

  constructor(private router: Router) { }
  
  ngOnInit(): void {
    if (this.router.url.includes('dashboard')) {
      this.selectedItem = 'Dashboard';
    }
    else if (this.router.url.includes('active')) {
      this.selectedItem = 'Active';
    }
    else if (this.router.url.includes('completed')) {
      this.selectedItem = 'Completed';
    }
  }

  toogleOptions() {
    this.myOptionRef.nativeElement.classList.toggle('show');
  }

  selectItem(event: Event) {
    const id = (event.target as HTMLInputElement).id as string;
    this.selectedItem = id;
    this.router.navigateByUrl(`/${id.toLowerCase()}`);
  }
}
