import { Pagina } from "../../../directive/models/page";
import { Message } from "../../../directive/models/message";

export class ActionResult {

  result: any;
  message: Message;
  success: boolean;
  page: Pagina;
}
