//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SqlService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Luggage
    {
        public string LuggageID { get; set; }
        public Nullable<float> Weight { get; set; }
        public string BoardingPassID { get; set; }
    
        public virtual BoardingPass BoardingPass { get; set; }
    }
}
