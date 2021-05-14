// Echo Client


#include "common.h"

using namespace std;

#define MAX_MSG_LEN 256
#define PORT_NUM 10200

void AcceptLoop(SOCKET sock);
void DoIt(SOCKET dosock);

int main()
{
    WSADATA wsadata;
    WSAStartup(MAKEWORD(2, 2), &wsadata);   // ���� �ʱ�ȭ
    char server_ip[40] = "";
    printf("Server IP:");
    gets_s(server_ip, sizeof(server_ip));
    SOCKET sock;
    sock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);// ���� ����
    SOCKADDR_IN servaddr = { 0 };       // server address
    servaddr.sin_family = AF_INET;
    servaddr.sin_addr.s_addr = inet_addr(server_ip);    // GetDefaultMyIP();
    servaddr.sin_port = htons(PORT_NUM);

    int re;
    re = connect(sock, (SOCKADDR*)&servaddr, sizeof(servaddr));   //���� �õ�
    if (re == -1) { return -1; }

    char msg[MAX_MSG_LEN]; // DoIt
    while (true)
    {
        cout << "Send : ";
        gets_s(msg, MAX_MSG_LEN);
        send(sock, msg, sizeof(msg), 0);
        if (strcmp(msg, "exit") == 0)
        {

            break;
        }
        recv(sock, msg, sizeof(msg), 0);
        printf("����: %s\n", msg);

     }
    closesocket(sock);
    WSACleanup();   // ���� ����ȭ





    return 0;
}

void DoIt(SOCKET dosock)
{

    int re;
    
    char msg[MAX_MSG_LEN] = "";
    while (recv(dosock, msg, sizeof(msg), 0) > 0)
    {
        printf("recv:%s\n", msg);
        send(dosock, msg, sizeof(msg), 0);
    }
    closesocket(dosock);
}