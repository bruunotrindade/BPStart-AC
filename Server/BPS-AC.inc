#if defined samp_rcon_included
	#endinput
#endif
#define samp_rcon_included
#if !defined socket_included
	#error You need socket.inc to use BPS-AC.inc
#endif

enum SocketAC
{
	Socket:acSocket,
	acTimer,
	acNome[24],
	acIP[16],
	acHashPC[129],
	bool:acConectado
};

static PlayerAC[MAX_PLAYERS][SocketAC];

enum
{
	ARQUIVOS_MODIFICADOS = 1,
	ARQUIVOS_SUSPEITOS = 2,
	AC_ADULTERADO = 3,
	AC_ANTIGO = 4,
	MULTIPLA_EXECUCAO = 5,
	FECHADO = 6
};

enum 
{
	BANDEIRA_CONEXAO = 'C',
	BANDEIRA_DESCONEXAO = 'D',
	BANDEIRA_ALERTA = 'A'
};

#define AC_VERSAO				"1.0"
#define AC_HASH					"9067a9ee2327fbe21d94cbcec81fec3b"
#define AC_DELAY				(5)
#define SOCKET_PORTA			(6000)

//Callback de quando o anti-cheater é conectado
forward OnPlayerACConnect(playerid, ip[], hash[]);
//Callback de quando o anti-cheater é desconectado
forward OnPlayerACDisconnect(playerid);
//Callback de quando o anti-cheater encontra algo
forward OnPlayerACResponse(playerid, cheat_code);
//Timer de verificação
public CheckPlayerAC(playerid);

//Sobrecarga da callback OnPlayerConnect
public OnPlayerConnect(playerid)
{
	new string[100], name[MAX_PLAYER_NAME+1];
	GetPlayerName(playerid, name, sizeof name);
	format(PlayerAC[playerid][acNome], sizeof(name), "%s", name);
	GetPlayerIp(playerid, PlayerAC[playerid][acIP], 16);

	PlayerAC[playerid][acConectado] = false;
	PlayerAC[playerid][acSocket] = socket_create(TCP);
	
	new res = socket_connect(PlayerAC[playerid][acSocket], PlayerAC[playerid][acIP], SOCKET_PORTA);
	
	if(res)
	{
		//Pacote inicial de conexão
		format(string, sizeof string, "%d|%s", playerid, PlayerAC[playerid][acNome]);
		res = SendPlayerACPackage(playerid, string);

		if(!res)
			socket_destroy(PlayerAC[playerid][acSocket]);
		else if(IsPlayerConnected(playerid))
		{
			//Se o pacote tiver chegado ao AC
			PlayerAC[playerid][acConectado] = true;
			PlayerAC[playerid][acTimer] = SetTimerEx("CheckPlayerAC", AC_DELAY * 1000, true, "i", playerid);
		}
	}

	#if defined SAMPAC_OnPlayerConnect
        SAMPAC_OnPlayerConnect(playerid);
    #endif

    return true;
}
#if defined _ALS_OnPlayerConnect
    #undef OnPlayerConnect
#else
    #define _ALS_OnPlayerConnect
#endif
#define OnPlayerConnect SAMPAC_OnPlayerConnect
#if defined SAMPAC_OnPlayerConnect
    forward SAMPAC_OnPlayerConnect(playerid);
#endif

//Sobrecarga da callback OnPlayerDisconnect
public OnPlayerDisconnect(playerid, reason)
{
	KillTimer(PlayerAC[playerid][acTimer]);
	OnPlayerACDisconnect(playerid);

	if(is_socket_valid(PlayerAC[playerid][acSocket]))
		socket_destroy(PlayerAC[playerid][acSocket]);

	#if defined SAMPAC_OnPlayerDisconnect
        SAMPAC_OnPlayerDisconnect(playerid, reason);
    #endif

    return true;
}
#if defined _ALS_OnPlayerDisconnect
    #undef OnPlayerDisconnect
#else
    #define _ALS_OnPlayerDisconnect
#endif
#define OnPlayerDisconnect SAMPAC_OnPlayerDisconnect
#if defined SAMPAC_OnPlayerDisconnect
    forward SAMPAC_OnPlayerDisconnect(playerid, reason);
#endif

//Callback de pacote recebido do AC cliente
public onSocketAnswer(Socket:id, data[], data_len)
{
	if(data_len > 0)
	{
		new index[5], aux = 0, str[32];
		//Pacote de conexão
		if(data[0] == BANDEIRA_CONEXAO)
		{
			for(new i = 0; i < 5; i++)
			{
				new aux2 = strfind(data, "|", true, aux+1);
				index[i] = aux2;
				aux = aux2;
			}

			strmid(str, data, index[0]+1, index[1]);
			new playerid = strval(str), hashAC[129], versao[32];
			strmid(PlayerAC[playerid][acHashPC], data, index[1]+1, index[2]);
			strmid(hashAC, data, index[2]+1, index[3]);
			strmid(versao, data, index[3]+1, index[4]);
		
			OnPlayerACConnect(playerid, PlayerAC[playerid][acIP], PlayerAC[playerid][acHashPC]);
			if(strcmp(hashAC, AC_HASH, false) != 0)
				if(strcmp(versao, AC_VERSAO, false) != 0)
					OnPlayerACResponse(playerid, AC_ANTIGO);
				else
					OnPlayerACResponse(playerid, AC_ADULTERADO);
		}
		//Pacote de alerta
		else if(data[0] == BANDEIRA_ALERTA)
		{
			for(new i = 0; i < 3; i++)
			{
				new aux2 = strfind(data, "|", true, aux+1);
				index[i] = aux2;
				aux = aux2;
			}

			strmid(str, data, index[0]+1, index[1]);
			new playerid = strval(str), codigo;
			strmid(str, data, index[1]+1, index[2]);
			codigo = strval(str);
		
			OnPlayerACResponse(playerid, codigo);
		}
		//Pacote de desconexão
		else if(data[0] == BANDEIRA_DESCONEXAO)
		{
			for(new i = 0; i < 2; i++)
			{
				new aux2 = strfind(data, "|", true, aux+1);
				index[i] = aux2;
				aux = aux2;
			}

			strmid(str, data, index[0]+1, index[1]);
			new playerid = strval(str);
		
			KillTimer(PlayerAC[playerid][acTimer]);
			OnPlayerACDisconnect(playerid);
		}
	}
	return true;
}

//Timer de verificação de funcionamento do AC cliente
public CheckPlayerAC(playerid)
{
	new result = SendPlayerACPackage(playerid, ".");

	if(result == -1 && IsPlayerConnected(playerid))
	{
		KillTimer(PlayerAC[playerid][acTimer]);
		OnPlayerACDisconnect(playerid);
	}	

	return true;
}

//Enviar pacote ao AC cliente
static SendPlayerACPackage(playerid, string[])
{
	return socket_send(PlayerAC[playerid][acSocket], string, strlen(string));
} 

public IsPlayerACConnected(playerid)
{
	return PlayerAC[playerid][acConectado];
}