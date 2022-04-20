import { Pagina } from './page';
import { Message } from './message';

export class TypedResult<T> {

  result: T;
  pagina: Pagina;
  message: Message;
  success: boolean;
}
