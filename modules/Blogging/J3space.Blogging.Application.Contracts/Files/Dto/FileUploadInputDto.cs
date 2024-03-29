﻿using System.ComponentModel.DataAnnotations;

namespace J3space.Blogging.Files.Dto
{
    public class FileUploadInputDto
    {
        [Required] public byte[] Bytes { get; init; }

        [Required] public string Name { get; init; }
    }
}