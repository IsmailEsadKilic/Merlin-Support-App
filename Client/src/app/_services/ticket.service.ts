import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Ticket, TicketAdd, TicketType, TicketTypeAdd } from '../_models/ticket';
import { Priority, PriorityAdd } from '../_models/priority';
import { TicketNode, TicketNodeAdd } from '../_models/ticketNode';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  baseUrl = environment.apiUrl + 'ticket';

  constructor(private Http: HttpClient) { }

  //ticket related

  getTickets() {
    return this.Http.get<Ticket[]>(this.baseUrl + '/ticket');
  }

  getTicket(id: number) {
    return this.Http.get<Ticket>(this.baseUrl + '/ticket/' + id);
  }

  addTicket(ticketAdd: TicketAdd) {
    return this.Http.put<Ticket>(this.baseUrl + '/ticket/add', ticketAdd);
  }

  updateTicket(ticketAdd: TicketAdd, ticketId: number) {
    return this.Http.post<Ticket>(this.baseUrl + '/ticket/update/' + ticketId, ticketAdd);
  }

  deleteTicket(ticketId: number) {
    return this.Http.delete(this.baseUrl + '/ticket/' + ticketId);
  }

  ///////////////////////////////////////////////////////////////ticket node related

  getTicketNodesByTicketId(ticketId: number) {
    return this.Http.get<TicketNode[]>(this.baseUrl + '/ticketNodesByTicketId/' + ticketId);
  }

  addTicketNode(ticketNodeAdd: TicketNodeAdd) {
    return this.Http.put<TicketNode>(this.baseUrl + '/ticketNode/add', ticketNodeAdd);
  }

  deleteTicketNode(ticketNodeId: number) {
    return this.Http.delete(this.baseUrl + '/ticketNode/' + ticketNodeId);
  }

  //ticket type related

  getTicketTypes() {
    return this.Http.get<TicketType[]>(this.baseUrl + '/ticketType');
  }

  getTicketType(id: number) {
    return this.Http.get<TicketType>(this.baseUrl + '/ticketType/' + id);
  }

  addTicketType(ticketTypeAdd: TicketTypeAdd) {
    return this.Http.put<TicketType>(this.baseUrl + '/ticketType/add', ticketTypeAdd);
  }

  updateTicketType(ticketTypeAdd: TicketTypeAdd, ticketTypeId: number) {
    return this.Http.post<TicketType>(this.baseUrl + '/ticketType/update/' + ticketTypeId, ticketTypeAdd);
  }

  deleteTicketType(ticketTypeId: number) {
    return this.Http.delete(this.baseUrl + '/ticketType/' + ticketTypeId);
  }

  ///////////////////////////////////////////////////////////////priority related

  getPriorities() {
    return this.Http.get<Priority[]>(this.baseUrl + '/priority');
  }

  getPriority(id: number) {
    return this.Http.get<Priority>(this.baseUrl + '/priority/' + id);
  }

  addPriority(priority: PriorityAdd) {
    return this.Http.put<Priority>(this.baseUrl + '/priority/add', priority);
  }

  updatePriority(priorityAdd: PriorityAdd, priorityId: number) {
    return this.Http.post<Priority>(this.baseUrl + '/priority/update/' + priorityId, priorityAdd);
  }

  deletePriority(priorityId: number) {
    return this.Http.delete(this.baseUrl + '/priority/' + priorityId);
  }

  ///////////////////////////////////////////////////////////////ticket state related

  getTicketStatesDict() {
    return this.Http.get<object>(this.baseUrl + '/ticketStatesDict');
  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////

  getTimeTypesDict() {
    return this.Http.get<object>(this.baseUrl + '/timeTypesDict');
  }

}
