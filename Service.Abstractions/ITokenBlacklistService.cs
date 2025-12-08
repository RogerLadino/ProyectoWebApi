using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

public interface ITokenBlacklistService
{
    void RevokeToken(string token);
    bool IsTokenRevoked(string token);
}
