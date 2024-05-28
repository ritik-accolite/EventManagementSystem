export interface PersonInterface {
  FirstName: string;
  LastName: string;
  Password: string;
  Role: string;
  IsBlocked: boolean;
  accessFailedCount: number;
  concurrencyStamp: string;
  email: string;
  emailConfirmed: boolean;
  firstName: string;
  id: string;
  lastName: string;
  lockoutEnabled: boolean;
  lockoutEnd: Date | null;
  normalizedEmail: string;
  normalizedUserName: string;
  password: string;
  passwordHash: string;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  role: string;
  securityStamp: string;
  twoFactorEnabled: boolean;
  userName: string;
}
