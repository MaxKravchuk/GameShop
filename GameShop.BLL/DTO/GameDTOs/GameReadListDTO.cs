﻿namespace GameShop.BLL.DTO.GameDTOs
{
    public class GameReadListDTO : GameBaseDTO
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string PhotoUrl { get; set; }
    }
}
