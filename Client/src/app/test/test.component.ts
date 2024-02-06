import { Component, OnInit } from '@angular/core';
import { TestService } from '../_services/test.service';
import { TestEntity } from '../_models/testEntity';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent implements OnInit {
  
  testEntities: TestEntity[] = [];
  
  constructor(private testService: TestService) { }
  
  ngOnInit(): void {
    this.testService.getAll().subscribe(testEntities => {
      console.log(testEntities);
      this.testEntities = testEntities;
    });
  }
}
