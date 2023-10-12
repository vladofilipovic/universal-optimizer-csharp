#include <stdio.h>
#include <stdlib.h>
#include <math.h>

#define STR_MAX 2000
#define LINIJA_MAX 2000
#define BROJ_LINIJA_MAX 5000

typedef struct cv {
	int id;
	char *oznaka;
} cvor;

typedef struct gr {
	int izvor_id;
	int odrediste_id;
	char *oznaka;
	double tezina;
} grana;

void main(int argc, char **argv)
{ 
	char imeul[STR_MAX];
	char imeizl[STR_MAX];
	FILE* ul;

	int broj_linija = 0;
	char linije[BROJ_LINIJA_MAX][LINIJA_MAX];

	int neorjentisan = 0;
	int broj_cvorova;
	int broj_grana;

	if( argc<2 )
	{
		printf("Nema dovoljno argumenata. \n Koriscenje: from_gml_to_plain <datoteka> \n\n");
		exit(0);
	}

	sprintf_s(imeul,STR_MAX, "%s.gml",argv[1]);
	//printf( "%s\n", imeul );
	sprintf_s(imeizl,STR_MAX, "%s.txt",argv[1]);
	//printf( "%s\n", imeizl );

	ul = fopen(imeul,"rt");
	if(ul==NULL)
	{
		printf("Ulazna datoteka ne moze da se otvori!!!!\n");
		exit(0);
	}  

	while( !feof(ul) )
		fgets( linije[broj_linija++], LINIJA_MAX, ul );


	printf("%s", "=== Pritisnite ENTER ===" ); getchar();
}