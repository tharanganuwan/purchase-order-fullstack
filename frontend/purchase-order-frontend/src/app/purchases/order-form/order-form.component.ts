import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PoService } from '../../services/po.service';
import { PurchaseOrder, PurchaseOrderStatus } from '../../models/purchase-order';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule
    
  ],
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.scss']
})
export class OrderFormComponent implements OnInit {
  editingId?: string;
  poForm: FormGroup;
  PurchaseOrderStatus = PurchaseOrderStatus;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private poService: PoService
  ) {
    this.poForm = this.fb.group({
      poNumber: ['', [Validators.required, Validators.maxLength(50)]],
      description: [''],
      supplierName: ['', [Validators.required, Validators.maxLength(200)]],
      orderDate: ['', Validators.required],
      totalAmount: [0, [Validators.required, Validators.min(0)]],
      status: ['Draft', Validators.required]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.editingId = id;
      this.poService.get(id).subscribe(po => {
        this.poForm.patchValue({
          poNumber: po.poNumber,
          description: po.description ?? '',
          supplierName: po.supplierName,
          orderDate: po.orderDate?.substring(0, 10), 
          totalAmount: po.totalAmount,
          status: po.status
        });
      });
    }
  }

  submit() {
    if (this.poForm.invalid) return;

    const dto = this.poForm.value as Partial<PurchaseOrder>;

    if (this.editingId) {
      this.poService.update(this.editingId, dto).subscribe(() => {
        alert('Purchase Order updated');
        this.router.navigate(['/purchases']);
      });
    } 
    else {
      this.poService.create(dto).subscribe(() => {
        alert('Purchase Order created');
        this.router.navigate(['/purchases']);
      });
    }
  }

  cancel() {
    this.router.navigate(['/purchases']);
  }
}
