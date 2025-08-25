import { Component, OnInit, ViewChild, inject  } from '@angular/core';
import { PurchaseOrder, PurchaseOrderStatus } from '../../models/purchase-order';
import {MatPaginator, PageEvent} from '@angular/material/paginator';
import { PoService } from '../../services/po.service';
import { CommonModule } from '@angular/common';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';




@Component({
  selector: 'app-order-list',
  imports: [CommonModule,MatPaginatorModule,MatSelectModule,
    MatFormFieldModule,FormsModule,RouterModule,MatTableModule,MatButtonModule,
    MatCardModule,MatInputModule,MatNativeDateModule,MatDatepickerModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.scss',
  standalone:true
})
export class OrderListComponent implements OnInit {


    supplierFilter = '';    
    displayedColumns: string[] = ['poNumber', 'supplierName', 'orderDate', 'totalAmount', 'status', 'actions'];
    dataSource: PurchaseOrder[] = [];

    total = 0;
    pageSize = 10;
    pageIndex = 0;
    statusFilter = '';
    fromDate?: Date;
    toDate?: Date;
    PurchaseOrderStatus = PurchaseOrderStatus;

    @ViewChild(MatPaginator) paginator!: MatPaginator;

     constructor(private poService: PoService) {}

    ngOnInit(): void {
      this.loadData();
    }

    loadData(): void {
      const params: any = {
        page: this.pageIndex + 1,
        pageSize: this.pageSize
      };

      if (this.supplierFilter) params.supplier = this.supplierFilter;
      if (this.statusFilter) params.status = this.statusFilter;

      if (this.fromDate) {
      const from = this.fromDate;
      const year = from.getFullYear();
      const month = (from.getMonth() + 1).toString().padStart(2, '0'); 
      const day = from.getDate().toString().padStart(2, '0');
      params.from = `${year}-${month}-${day}`;
    }

    if (this.toDate) {
      const to = this.toDate;
      const year = to.getFullYear();
      const month = (to.getMonth() + 1).toString().padStart(2, '0');
      const day = to.getDate().toString().padStart(2, '0');
      params.to = `${year}-${month}-${day}`;
    }

      this.poService.getList(params).subscribe(res => {
        this.dataSource = res.items;
        this.total = res.totalCount;
      });
    }

    onPageChange(event: PageEvent) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
      this.loadData();
    }

    clearFilters() {
      this.supplierFilter = '';
      this.statusFilter = '';
      this.fromDate = undefined;
      this.toDate = undefined;
      this.pageIndex = 0;
      this.loadData();
    }

    delete(po: PurchaseOrder) {
      if (!confirm(`Delete PO ${po.poNumber}?`)) return;
     this.poService.delete(po.id).subscribe(() => this.loadData());
    }
}
