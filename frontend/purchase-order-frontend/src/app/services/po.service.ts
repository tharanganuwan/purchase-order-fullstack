import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PurchaseOrder, PagedResult } from '../models/purchase-order';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PoService {
  private baseUrl = `${environment.apiBase}/purchaseorders`;

  constructor(private http: HttpClient) {}

  getList(params: any): Observable<PagedResult<PurchaseOrder>> {
    let httpParams = new HttpParams();
    Object.keys(params || {}).forEach(k => {
      if (params[k] !== undefined && params[k] !== null) {
        httpParams = httpParams.set(k, params[k]);
      }
    });
    return this.http.get<PagedResult<PurchaseOrder>>(this.baseUrl, { params: httpParams });
  }

  get(id: string): Observable<PurchaseOrder> {
    return this.http.get<PurchaseOrder>(`${this.baseUrl}/${id}`);
  }

  create(dto: Partial<PurchaseOrder>): Observable<PurchaseOrder> {
    return this.http.post<PurchaseOrder>(this.baseUrl, dto);
  }

  update(id: string, dto: Partial<PurchaseOrder>): Observable<PurchaseOrder> {
    return this.http.put<PurchaseOrder>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
