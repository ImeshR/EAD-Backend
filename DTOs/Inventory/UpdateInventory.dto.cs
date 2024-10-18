/*
 * File: UpdateInventoryDto
 * Author: Fernando B.K.M.
 * Description: This file defines the Data Transfer Object (DTO) used for updating inventory details.
 */

namespace EAD_Backend.DTOs
{
    public class UpdateInventoryDto
    {
        public int QuantityAvailable { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
