using System;

namespace ZwajApp.Api.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId{ get; set; }
        public User Sender { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDelelted { get; set; }
        public bool RecipientDelelted { get; set; }
        public DateTime? DateRead   { get; set; }
    }
}