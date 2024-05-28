import { GetAllPersonsByAdminInterface } from './get-all-persons-by-admin-interface';

export interface GetPersonByRoleInterface {
  status: number;
  message: string;
  allPersons: GetAllPersonsByAdminInterface[];
}
