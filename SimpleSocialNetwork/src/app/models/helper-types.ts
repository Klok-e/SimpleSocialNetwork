import {CommentModel, LimitedUserModel, OpMessageModel, UserModel} from '../../backend_api_client';

export type UnionUserModel =
  (LimitedUserModel & { modelType: 'limited' }) |
  (UserModel & { modelType: 'full' });

export type OpMessageUserDeleted = OpMessageModel & { posterIsDeleted: boolean };

export type CommentUserDeleted = CommentModel & { commenterIsDeleted: boolean };
