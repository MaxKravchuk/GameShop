import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UtilsService } from "../helpers/utilsService/utils-service";
import { catchError, Observable } from "rxjs";
import { CreateOrderModel } from "../../models/CreateOrderModel";
import { PaymentCreateDTO } from "../../models/PaymentCreateDTO";

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

    private apiUrl: string = "/api/payments";

    constructor(
      private http: HttpClient,
      private utilsService: UtilsService
    ) { }

    getInvoice(paymentCreateDTO: PaymentCreateDTO): Observable<any> {
        const headers: HttpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/octet-stream'
        });

        const url: string = `${this.apiUrl}/payAndGetInvoice`;
        return this.http.post(url, paymentCreateDTO, {headers: headers, responseType: 'blob' })
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }

    getOrderId(paymentCreateDTO: PaymentCreateDTO): Observable<number> {
        const url: string = `${this.apiUrl}/pay`;
        return this.http.post<number>(url, paymentCreateDTO)
            .pipe(
                catchError(err => {
                    this.utilsService.handleError(err);
                    return [];
                })
            );
    }
}
