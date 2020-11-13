export * from './auth.service';
import { AuthService } from './auth.service';
export * from './opMessage.service';
import { OpMessageService } from './opMessage.service';
export const APIS = [AuthService, OpMessageService];
