﻿namespace GameShop.DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
