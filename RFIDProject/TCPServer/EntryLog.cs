//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TCPServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntryLog
    {
        public int IDEntry { get; set; }
        public Nullable<int> RfIdUserId { get; set; }
        public Nullable<int> RfIdReaderId { get; set; }
        public System.DateTime EntryTime { get; set; }
    
        public virtual RfIdReader RfIdReader { get; set; }
        public virtual RfIdUser RfIdUser { get; set; }
    }
}
