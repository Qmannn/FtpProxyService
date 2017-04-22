﻿using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface IUserDtoConverter
    {
        List<UserDto> Convert( List<User> users );
        UserDto Convert( User user );
    }
}