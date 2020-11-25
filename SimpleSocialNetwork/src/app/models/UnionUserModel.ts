import {LimitedUserModel, UserModel} from '../../backend_api_client';

export type UnionUserModel =
  (LimitedUserModel & { modelType: 'limited' }) |
  (UserModel & { modelType: 'full' });
