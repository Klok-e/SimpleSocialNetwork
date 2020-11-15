export * from './auth.service';
import { AuthService } from './auth.service';
export * from './opMessage.service';
import { OpMessageService } from './opMessage.service';
export * from './user.service';
import { UserService } from './user.service';
export const APIS = [AuthService, OpMessageService, UserService];
