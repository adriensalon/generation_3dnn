

# génération procédurale
### principe :
l'ambition était de pouvoir générer des objets procéduralement, en fonction d'un "type" et de dimensions souhaités. pour cela il faut fournir pour chaque "type" d'objet une collection de modèles 3d : 
- les géométries sont découpées pour pouvoir être reconstituées à la taille désirée
- les textures servent à l'entraînement de réseaux de neurones [pix2pix](https://phillipi.github.io/pix2pix/) travaillant sur des assemblages de 6 projections orthogonales. le rendu nécessite un shader triplanaire.
 
l'objectif était de créer des univers de jv en temps réel. 
### notes :

 - l'implémentation utilise [matlab runtime](https://fr.mathworks.com/products/compiler/matlab-runtime.html) pour les réseaux de neurones pix2pix, il est donc nécessaire de le redistribuer.

- */editeur* est un exemple utilisant unity 2019.3 pour
	-  	importer depuis .obj, .jpg, et visualiser les découpes
	- entraîner les réseaux de neurones et visualiser les résultats
	- générer des scènes depuis un éditeur de scripts lua
- des extern sont définis pour utiliser l'api dans unity par exemple.

fonctions :

    extern "C"
    {
    	geometrie_procedurale_api
    	void unity_creer_projet(
    		char* url_dossier,
    		callback_log cb_log);
    
    	geometrie_procedurale_api
    	void unity_charger_projet(
    		char* url_dossier,
    		callback_log cb_log);
    
    	geometrie_procedurale_api
    	void unity_obtenir_noms_scripts(
    		callback_liste_string callback);
    
    	geometrie_procedurale_api
    	void unity_obtenir_noms_categories(
    		callback_liste_string callback);
    
    	geometrie_procedurale_api
    	void unity_obtenir_noms_geometries(
    		std::string categorie,
    		callback_liste_string callback);
    
    	geometrie_procedurale_api
    	void unity_obtenir_noms_ias(
    		std::string categorie,
    		callback_liste_string callback);
    		
    	geometrie_procedurale_api
    	void unity_calculer_geometrie(
    		int nombre_vertices,
    		float* positions_x, float* positions_y, float* positions_z,
    		float* normales_x, float* normales_y, float* normales_z,
    		float* uv_x, float* uv_y,
    		int nombre_triangles,
    		int* indices_x, int* indices_y, int* indices_z,
    		float* ajustement_dim_x,
    		float* ajustement_taille);
    
    	geometrie_procedurale_api
    	void unity_sauvegarder_geometrie(
    		char* categorie,
    		char* nom_fichier);
    		
    	geometrie_procedurale_api
    	void unity_generer_geometrie_editeur(
    		callback_geometrie callback,
    		float* taille);
    		
    	geometrie_procedurale_api
    	void unity_generer_geometrie(
    		char* nom_sauvegarde,
    		callback_geometrie callback,
    		float* taille);
    		
    	geometrie_procedurale_api
    	void unity_commencer_entrainement_texture(
    		char* nom_categorie,
    		char* nom_ia,
    		int utiliser_gpu,
    		int continuer);
    
    	geometrie_procedurale_api
    	void unity_obtenir_resultats_entrainement_texture(
    		callback_resultats_entrainement_texture cb);
    
    	geometrie_procedurale_api
    	void unity_arreter_entrainement_texture();
    
    	geometrie_procedurale_api
    	void unity_generer_texture(
    		char* nom_categorie,
    		char* nom_ia,
    		char* url_label,
    		char* url_target,
    		int utiliser_gpu);
    
    	geometrie_procedurale_api
    	void unity_executer_script(
    		char* nom_script,
    		callback_photographie cb_photo,
    		callback_geometrie cb_geo);
    }

callbacks :

    typedef void (*callback_log)(
    	size_t taille_message,
    	void* message);
    
    typedef void (*callback_liste_string)(
    	size_t taille_chaine, char* chaine,
    	size_t taille_indices, void* indices);
    
    typedef char* (*callback_resultats_entrainement_texture)(
    	int epochs,
    	float gLoss,
    	float dLoss,
    	float lossL1,
    	float ganLoss);
    
    typedef void (*callback_photographie)(
    	size_t taille_url_texture_predict_label,
    	size_t taille_url_texture_predict_target,
    	char* url_texture_predict_label,
    	char* url_texture_predict_target);
    
    typedef void (*callback_geometrie)(
    	size_t nombre_vertices,
    	size_t nombre_triangles,
    	void* positionsX, void* positionsY, void* positionsZ,
    	void* normalesX, void* normalesY, void* normalesZ,
    	void* uvsX, void* uvsY,
    	void* indices);



## ouvrir un projet
l'api a besoin d'un dossier local pour stocker les données importées, les fichiers calculés et les réseaux de neurones pix2pix entraînés.

    void creer_projet(
    	const std::string& url_dossier)

on peut ouvrir un projet déjà créé précédemment.

    void charger_projet(
		const std::string& url_dossier)

un projet contient plusieurs *catégories* d'objet. lorsqu'on voudra générer un objet, il faudra spécifier sa catégorie. un réseau de neurones ne peut être entraîné qu'avec des objets de même catégorie et il faudra choisir des modèles 3d relativement "similaires" en terme de textures. toutes les images *label* d'une catégorie devront utiliser le même code couleur.

## importer des données
pour chaque objet 3 fichiers sont nécessaires :

 - un fichier géométrie .obj, .fbx... converti en structure **géométrie**.


les types **reel2**, **entier3**... permettent d'effectuer une partie des opérations avec des instructions SSE2.

    std::vector<reel3> positions
    std::vector<reel3> normales
    std::vector<reel2> uvs
    std::vector<entier3> indices

 il est possible d'importer directement des fichiers .off pour obtenir la **géométrie**.

    geometrie charger_fichier_off(
    	const std::string& nom_fichier)

 - un fichier image .jpg *label* représentant avec des régions de couleurs la texture, samplée par les uvs de la géométrie.
 - un fichier image .jpg *target* représentant la texture désirée, samplée par les uvs de la géométrie.

les réseaux de neurones doivent être entraînés en leur fournissant des paires d'images *label* et *target*. on peut donc "photographier" les géométries au moment de les importer, en assemblant les projections de 6 caméras orthogonales formant un cube pour obtenir ces images *label* et *target*. pour ces deux fichiers, on fournit directement l'url à matlab.

la géométrie doit être calculée une seule fois pour pouvoir servir plus tard à la génération

    sauvegarde_geometrie calculer_geometrie(
    	const geometrie& g, 
    	const parametres_calcul_geometrie& p)` 

la structure **parametres_calcul_geometrie** permet de définir les plans de découpe 1d, 2d, ou 3d pour pouvoir plus tard adapter la géométrie selon les dimensions voulues.

		

    float boite_x
    float boite_y
    float boite_z
    float ajustement_debut_x
    float ajustement_extension_x
    float ajustement_fin_x


[todo expliquer plus]



## entraîner la génération de textures 
l'entraînement se fait dans un thread séparé car l'intervalle entre 2 ticks peut durer plusieurs secondes. le paramètre **nom_ia** définit un tag pour le réseau de neurone, un nouveau réseau sera initialisé si le tag n'a jamais été utilisé. on peut donc entraîner plusieurs réseaux à partir des mêmes données.

    void commencer_entrainement_texture(
		const std::string& nom_categorie,
		const std::string& nom_ia,
		const parametres_entrainement_texture& p)`

la structure **parametres_entrainement_texture** définit des paramètres supplémentaires.
	
    bool continuer
    bool utiliser_gpu

le réseau de neurones est sauvegardé sur le disque à chaque tick et l'entraînement peut donc être repris plus tard. l'implémentation matlab permet d'utiliser des kernels CUDA pour l'entraînement, non testé sur AMD. les résultats peuvent être consultés à tout moment.
    
    resultats_entrainement_texture obtenir_resultats_entrainement_texture()

la structure **resultats_entrainement_texture** fournit des informations sur l'entraînement du réseau de neurones. 

    bool nouveau_tick
   	int epoch
   	float gLoss
   	float dLoss
   	float lossL1
   	float ganLoss

**nouveau_tick** indique si un nouveau tick est passé depuis le dernier appel à la fonction. on peut arrêter l'entraînement à tout moment.

	void interrompre_entrainement_texture()


## générer des objets
pour générer la géométrie il suffit d'un fichier sauvegarde et des dimensions souhaitéees, le fichier est sélectionné pour s'adapter au mieux au dimensions demandées. on pourra sauvegarder la géométrie obtenue en fichier .obj, .fbx...

    geometrie generer_geometrie(
		const sauvegarde_geometrie& s,
		const float taille_x)` 

il est possible de sauvegarder la géométrie obtenue directement en fichier .off

    void sauvegarder_fichier_off(
    	const std::string& nom_fichier,
    	const geometrie& g)

les réseaux de neurones peuvent générer une texture *target* à partir d'une texture *label*. pour l'obtenir, il faut avec la même technique que pour l'importation prendre en photo la géométrie générée.

    void generer_texture(
    	const std::string& nom_categorie,
    	const std::string& nom_ia,
    	const std::string& url_tex_label,
    	const std::string& url_tex_target,
    	const bool utiliser_gpu)



on peut finalement projeter la texture *target* obtenue en la découpant en 6 textures natives correspondant aux 6 projections, et avec un shader triplanaire on obtient l'objet final.

    Shader "6planar"
    {
	      Properties
	       {
              [NoScaleOffset] _xMoins ("X Moins", 2D) = "white" {}
              [NoScaleOffset] _xPlus ("X Plus", 2D) = "white" {}
              [NoScaleOffset] _yMoins ("Y Moins", 2D) = "white" {}
              [NoScaleOffset] _yPlus ("Y Plus", 2D) = "white" {}
              [NoScaleOffset] _zMoins ("Z Moins", 2D) = "white" {}
              [NoScaleOffset] _zPlus ("Z Plus", 2D) = "white" {}
              
      		_CentreX ("Centre X", Float) = 1
      		_CentreY ("Centre Y", Float) = 1
      		_CentreZ ("Centre Z", Float) = 1
  
   		_TailleX ("Taille X", Float) = 1
   		_TailleY ("Taille Y", Float) = 1
   		_TailleZ ("Taille Z", Float) = 1
   
   		_Taille ("Taille", Float) = 1
   		_MelangeDecalage ("Decalage", Range(0, 0.5)) = 0.25
   		_MelangeExposant ("Exposant", Range(1, 8)) = 2
       }
       SubShader
       {
   		Pass
   		{
   			Tags { "LightMode" = "ForwardBase" }
   			Cull back
   			
   			CGPROGRAM    
  				#include "include_vertex.cginc"
  				#include "include_lumiere.cginc"
  
  				#pragma vertex vert
  				#pragma fragment frag             
  				#pragma multi_compile_fwdbase
  				#pragma target 3.0
  
  				uniform sampler2D _xMoins;
  				uniform sampler2D _xPlus;
  				uniform sampler2D _yMoins;
  				uniform sampler2D _yPlus;
  				uniform sampler2D _zMoins;
  				uniform sampler2D _zPlus;
  
  				uniform float _CentreX;
  				uniform float _CentreY;
  				uniform float _CentreZ;
  				uniform float _TailleX;
  				uniform float _TailleY;
  				uniform float _TailleZ;
  				uniform float _MelangeDecalage;
  				uniform float _MelangeExposant;
  				    
  				struct appdata
  				{
  					float4 vertex : POSITION;
  					float3 normal : NORMAL;
  				};    
  				struct v2f
  				{
  					float4 pos : SV_POSITION;
  					float3 worldNormal : TEXCOORD0;
  					float3 worldPosition : TEXCOORD1;
  					float3 viewDir : TEXCOORD2;
  					LIGHTING_COORDS(5,8)
  				};
  
  				v2f vert(appdata v)
  				{
  					v2f o;
  					CALCULER_VERTEX(v.vertex, v.normal, o.pos, o.worldPosition, o.worldNormal, o.viewDir);	
  					TRANSFER_VERTEX_TO_FRAGMENT(o);
  					return o;
  				}
  
  				float4 frag(v2f i) : SV_TARGET
  				{
  					// uvs
		    	float3 p = i.worldPosition;// * _Taille;
		    	p.x = (p.x - _CentreX) / _TailleX;
		    	p.y = (p.y - _CentreY) / _TailleY;
		    	p.z = (p.z - _CentreZ) / _TailleZ;
		       
		    	float2 uvX = p.zy;
		    	float2 uvY = p.xz;
		    	float2 uvZ = p.xy;
		    	bool3 bPlusOuMoins = bool3(true, true, true);
		    	if (i.worldNormal.x < 0)
		    	{
		    		uvX.x = -uvX.x;
		    		bPlusOuMoins.x = false;
		    	}
		    	if (i.worldNormal.y < 0)
		    	{
		    		uvY.x = -uvY.x;
		    		bPlusOuMoins.y = false;
		    	}
		    	if (i.worldNormal.z >= 0)
		    	{
		    		uvZ.x = -uvZ.x;
		    		bPlusOuMoins.z = false;
		    	}
		    	
		    	// melange (hauteurs et normales)
		    	float3 triW = abs(i.worldNormal);
		    	triW = saturate(triW - _MelangeDecalage);
		    	triW = pow(triW, _MelangeExposant);
		    	triW = triW / (triW.x + triW.y + triW.z);
		    
		    	// albedo
		    	float3 albedoX = bPlusOuMoins.x ? tex2D(_xPlus, uvX).rgb : tex2D(_xMoins, uvX).rgb;
		    	float3 albedoY = bPlusOuMoins.y ? tex2D(_yPlus, uvY).rgb : tex2D(_yMoins, uvY).rgb;
		    	float3 albedoZ = bPlusOuMoins.z ? tex2D(_zPlus, uvZ).rgb : tex2D(_zMoins, uvZ).rgb;
		    
		    	// output
		    	float3 albedo = albedoX * triW.x + albedoY * triW.y + albedoZ * triW.z;
  
  					float3 lumiere;
  					fixed atten = LIGHT_ATTENUATION(i);
  					
  					float4 col;
  					col.rgb = albedo * 1.7;// * atten;
  					col.a = 1;
  					return col;
  				}
  			ENDCG
          }
  	}
  
*(exemple shaderlab unity 2019.3)*

**todo** : on peut générer des maps supplémentaires avec [materialize](https://github.com/BoundingBoxSoftware/Materialize) directement depuis l'image *target* (normal, height, smoothness, occlusion...)

