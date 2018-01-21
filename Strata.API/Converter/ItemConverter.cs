using Strata.API.Model.Shopping;
using Strata.Data.Shopping;

namespace Strata.API.Converter {
    public static class ItemConverter {

        public static ItemDTO ToDTO(this Item item) {
            return new ItemDTO {
                Cost = item.Cost,
                Name = item.Name
            };
        }
    }
}
