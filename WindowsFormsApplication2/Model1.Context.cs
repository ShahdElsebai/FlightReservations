﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApplication2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class projectEntities : DbContext
    {
        public projectEntities()
            : base("name=projectEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        [DbFunction("projectEntities", "Display_flight")]
        public virtual IQueryable<Display_flight_Result> Display_flight(string input_Departure, string input_distnation, Nullable<System.DateTime> departureTime, Nullable<int> num_seats)
        {
            var input_DepartureParameter = input_Departure != null ?
                new ObjectParameter("input_Departure", input_Departure) :
                new ObjectParameter("input_Departure", typeof(string));
    
            var input_distnationParameter = input_distnation != null ?
                new ObjectParameter("input_distnation", input_distnation) :
                new ObjectParameter("input_distnation", typeof(string));
    
            var departureTimeParameter = departureTime.HasValue ?
                new ObjectParameter("DepartureTime", departureTime) :
                new ObjectParameter("DepartureTime", typeof(System.DateTime));
    
            var num_seatsParameter = num_seats.HasValue ?
                new ObjectParameter("num_seats", num_seats) :
                new ObjectParameter("num_seats", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Display_flight_Result>("[projectEntities].[Display_flight](@input_Departure, @input_distnation, @DepartureTime, @num_seats)", input_DepartureParameter, input_distnationParameter, departureTimeParameter, num_seatsParameter);
        }
    
        [DbFunction("projectEntities", "Displayflightn")]
        public virtual IQueryable<Displayflightn_Result> Displayflightn(string input_Departure, string input_distnation, Nullable<System.DateTime> departureTime)
        {
            var input_DepartureParameter = input_Departure != null ?
                new ObjectParameter("input_Departure", input_Departure) :
                new ObjectParameter("input_Departure", typeof(string));
    
            var input_distnationParameter = input_distnation != null ?
                new ObjectParameter("input_distnation", input_distnation) :
                new ObjectParameter("input_distnation", typeof(string));
    
            var departureTimeParameter = departureTime.HasValue ?
                new ObjectParameter("DepartureTime", departureTime) :
                new ObjectParameter("DepartureTime", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Displayflightn_Result>("[projectEntities].[Displayflightn](@input_Departure, @input_distnation, @DepartureTime)", input_DepartureParameter, input_distnationParameter, departureTimeParameter);
        }
    }
}
