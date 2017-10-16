/* A simple server in the internet domain using TCP
   The port number is passed as an argument */
#include <stdio.h>
#include <sys/types.h> 
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdlib.h>
#include <strings.h>
#include <unistd.h>
#include <gmp.h>

void error(char *msg)
{
    perror(msg);
    exit(1);
}

int main(int argc, char *argv[])
{
     int sockfd, newsockfd, portno, clilen;
     mpz_t rsn, rsd, pt, ct;

     char buffer[256], psbuffer[256];
     struct sockaddr_in serv_addr, cli_addr;
     int n;
     if (argc < 2) {
         fprintf(stderr,"ERROR, no port provided\n");
         exit(1);
     }
     sockfd = socket(AF_INET, SOCK_STREAM, 0);
     if (sockfd < 0) 
        error("ERROR opening socket");
     bzero((char *) &serv_addr, sizeof(serv_addr));
     portno = atoi(argv[1]);
     serv_addr.sin_family = AF_INET;
     serv_addr.sin_addr.s_addr = INADDR_ANY;
     serv_addr.sin_port = htons(portno);
     if (bind(sockfd, (struct sockaddr *) &serv_addr,
              sizeof(serv_addr)) < 0) 
              error("ERROR on binding");
     listen(sockfd,5);
     clilen = sizeof(cli_addr);
     newsockfd = accept(sockfd, (struct sockaddr *) &cli_addr, &clilen);
     if (newsockfd < 0) 
          error("ERROR on accept");
     bzero(buffer,256);
     n = read(newsockfd,buffer,255);
     
     if (n < 0) error("ERROR reading from socket");
    

     mpz_init(pt);
     mpz_init(ct);
     mpz_init_set_str(rsn, "9516311845790656153499716760847001433441357", 10);
     mpz_init_set_str(rsd, "5617843187844953170308463622230283376298685", 10);

     mpz_set_str(ct, buffer, 10);
     
     gmp_printf("Encoded:   %Zd\n", ct);
     mpz_powm(pt, ct, rsd, rsn);
     gmp_printf("Decoded:   %Zd\n", pt);

     mpz_export(psbuffer, NULL, 1, 1, 0, 0, pt);


     printf("Here is the message: %s\n",psbuffer);
     n = write(newsockfd,"I got your message",18);
     if (n < 0) error("ERROR writing to socket");
     
     close(newsockfd);
     close(sockfd);

     return 0; 
}
