using System.ComponentModel.DataAnnotations;

namespace Strata.API.Model.Shopping {
    public class UpdateQuantityDTO {

        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }

        [Range(1, int.MaxValue)]
        public int NewQuantity { get; set; }
    }
}
