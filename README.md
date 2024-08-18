# Seas-of-siege

1) Geração de grid
	Cria o matriz que ira definir o mapa do jogo, com os spawnpoints de inimigos, a ilha inicial juntamente com o personagem do jogador
2) Geração de terreno
	Insere no mapa as assets do jogador e do terreno (árvores, pedras, etc).
3) Inicia a contagem das "ondas"
	Cada onda contém uma quantidade de inimigos a serem derrotadas pelo jogador, após vencer cada "onda" o jogador pode comprar torres ou expandir a sua ilha
4) Criação de inimigos
	Durante as ondas, inimigos são criados até o limite estipulado pela fase atual, contada no passo anterior, cada inimigo pode assumir uma das assets configuradas, efeito apenas visual. Após a criação o inimigo identifica o terreno mais próximo de seu spawn point e traça uma linha reta até ela, após alcança-la começa a destruir o terreno em uma quantidade de ataques, caso a saúde do terreno chegue a 0, ele é destruído
5) Execução dos scripts de jogabilidade
	Durante os ataques, o jogador possui a capacidade de atacar os inimigos que atacam seu terreno, tais ataques são automáticos e dependem apenas da proximidade do jogador, os inimigos possuem o script de sempre atacar o terreno mais próximo e fornecem moedas ao jogador ao serem abatidos, assim como as torres atacam os inimigos mais próximos a ela.
6) Fim do turno de combate
	Após vencer todos os inimigos, o turno de construção inicia, podendo criar torres e ilhas, e deve ser encerrada pelo próprio jogador.
7) Continua a contagem de ondas
	A execução continua como o passo 3, aumentando o número de inimigos até a derrota do jogador.
8) Condições de derrota do jogador
	O jogo é encerrado a partir do momento que o terreno em que o jogador se encontra é destruído enquanto ele permanece nela.
9) Tipos de torres
	Existem 3 tipos de torres para auxiliar o jogador, descritas a seguir:
	a) Mcc
		Torre que atira em um inimigo por vez, a mais básica das três
	b) Mcc B
		Torre que atira uma bomba de dano em área, possui dano e alcance reduzido comparado com a torre Mcc A
	c) Mcc C
		Torre que atira em um inimigo por vez, causa muito mais dano, porém é mais lenta que a Mcc A, possui o maior alcance entre as 3 torres.
	Cada torre pode receber 3 níveis de melhoria, aumentando seu dano, velocidade de ataque e alcance.
10) Regras do turno de construção
	No turno de construção o jogador pode gastar as moedas que adquiriu dos inimigos de duas formas, colocando ilhas, ou torres nas ilhas já existentes, seguindo as seguintes regras:
	a) Colocação de ilhas
		Para ser construida uma ilha, ela deve possuir uma conexão direta com a ilha principal, ilhas que perdem a conexão por ataques inimigos são destruídas.
	b) Colocação de torres
		Para construir uma torre, é necessário possuir uma ilha sem torre, gastando uma quantidade de moedas o jogador pode preencher essa ilha com uma das 3 variedades de torres, e após colocadas elas podem ser melhoradas com moedas ou vendidas por um preço menor do que o de aquisição
