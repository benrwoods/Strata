using System.ComponentModel.DataAnnotations;

namespace Strata.API.Model.Shopping {
    public class DeleteItemDTO {

        [Range(1, int.MaxValue)]
        public int ItemId { get; set; }
    }
}
