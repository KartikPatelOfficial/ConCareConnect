using System;

namespace LaboratoryPanelWPF.Model
{
    public class Laboratory
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Logo { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public DateTime? Createdat { get; set; }

        public string? Status { get; set; }

        public string? Sign { get; set; }

        public string Addressline1 { get; set; } = null!;

        public string Addressline2 { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Pincode { get; set; } = null!;

        public string Firebaseuid { get; set; } = null!;

    }
}
