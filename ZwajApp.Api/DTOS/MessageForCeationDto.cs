using System;

namespace ZwajApp.Api.DTOS
{
    public class MessageForCeationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent{ get; set; }
        public string Content { get; set; }
        public MessageForCeationDto()
        {
   MessageSent = DateTime.Now;
  }
    }
}