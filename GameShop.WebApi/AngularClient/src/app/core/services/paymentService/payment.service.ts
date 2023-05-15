import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { catchError, Observable } from "rxjs";
import { CreateOrderModel } from "../../models/CreateOrderModel";

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

    private apiUrl: string = "/api/payments";

    constructor(
      private http: HttpClient,
      private utilsService: UtilsService
    ) { }

    getInvoice(orderCreateDTO: CreateOrderModel): Observable<any> {
        const headers: HttpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/octet-stream'
        });

        const url = `${this.apiUrl}/pay`;
        return this.http.post(url, orderCreateDTO, {headers: headers, responseType: 'blob' })
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getOrderId(orderCreateDTO: CreateOrderModel): Observable<number> {
        const url = `${this.apiUrl}/pay`;
        return this.http.post<number>(url, orderCreateDTO)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
