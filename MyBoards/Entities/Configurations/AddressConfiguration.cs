using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace MyBoards.Entities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.OwnsOne(c => c.Coordinate, onb =>
                {
                    onb.Property(l => l.Latitude)
                        .HasPrecision(18, 7);
                    onb.Property(l => l.Longitude)
                        .HasPrecision(18, 7);
                });
        }
    }
}
