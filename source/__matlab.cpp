#include "GPROC.h"

#include <textures_ia.h>

void construction_matlab()
{
	if (!mclInitializeApplication(NULL, 0)) {
		log("could not initialize the application");
		return;
	}
	if (!textures_iaInitialize()) {
		log("erreur : [Matlab] could not initialize the library");
		return;
	}
}

void destruction_matlab()
{
	textures_iaTerminate();
	mclTerminateApplication();
	return;
}