# Seas of siege

## Colaboradores

- [@FreseEnzo](https://github.com/LeoKeiji) - Frese
- [@LeoKeiji](https://github.com/FreseEnzo) - Keiji

Seas of siege é um jogo de torre de defesa contínuo desenvolvido com Unity e C#. Defenda sua ilha contra ondas de inimigos, expanda seu território e coloque estrategicamente torres para sobreviver o máximo possível!

[Bibliteca gráfica (Unity 3D)](unity_tutorial.md).

## Sumário
1. [Motivação](#motivação)
2. [Visão Geral do Jogo](#visão-geral-do-jogo)
3. [Recursos](#recursos)
4. [Começando](#começando)
   - [Pré-requisitos](#pré-requisitos)
   - [Instalação](#instalação)
5. [Jogabilidade](#jogabilidade)
   - [Geração de Grade](#geração-de-grade)
   - [Geração de Terreno](#geração-de-terreno)
   - [Sistema de Ondas](#sistema-de-ondas)
   - [Criação de Inimigos](#criação-de-inimigos)
   - [Mecânicas de Jogo](#mecânicas-de-jogo)
   - [Fase de Construção](#fase-de-construção)
   - [Tipos de Torres](#tipos-de-torres)
   - [Condições de Fim de Jogo](#condições-de-fim-de-jogo)
6. [Desenvolvimento](#desenvolvimento)
7. [Licença](#licença)

# Motivação

1. **Profundidade Estratégica**: O cenário de ilha fornece um ambiente natural e contido para os jogadores exercitarem o pensamento estratégico. O espaço limitado força os jogadores a tomar decisões significativas sobre posicionamento e alocação de recursos.

2. **Potencial Narrativo**: O cenário permite uma narrativa envolvente, com a possibilidade de introduzir personagens coloridos tanto do lado colonial quanto do lado dos piratas, adicionando profundidade à experiência de jogo.

3. **Rejogabilidade**: A combinação de eventos aleatórios, ataques variados de piratas e múltiplas estratégias para desenvolvimento da ilha garante alta rejogabilidade, mantendo os jogadores engajados ao longo do tempo.

4. **Oportunidade de Mercado**: Enquanto os jogos de torre de defesa são populares, o cenário único e os elementos de guerra naval ajudam a diferenciar este jogo em um mercado lotado.

## Visão Geral do Jogo

Seas of siege é um emocionante jogo de torre de defesa onde os jogadores devem proteger sua ilha de ondas implacáveis de inimigos. Expanda seu território, construa torres poderosas e use estratégia para sobreviver o máximo possível neste desafio contínuo.

![Screenshot from 2024-08-27 21-54-09](https://github.com/user-attachments/assets/2d6508d1-84cc-4678-abf3-aad1c05bb97b)
![Screenshot from 2024-08-27 21-54-33](https://github.com/user-attachments/assets/b58d1303-b4fb-42f7-810a-e52c2eea36cc)
![Screenshot from 2024-08-27 21-54-42](https://github.com/user-attachments/assets/bcac092d-e523-478c-ba09-7482e8246355)
![Screenshot from 2024-09-03 21-50-46](https://github.com/user-attachments/assets/9d072acd-4854-47cd-9174-d92309606895)

## Recursos

- Mapa gerado proceduralmente baseado em grade
- Terreno dinâmico com elementos destrutíveis
- Sistema de spawning de inimigos por ondas
- Personagem controlado pelo jogador com capacidade de ataque automático
- Três tipos únicos de torres com possibilidade de upgrade
- Mecânicas estratégicas de expansão da ilha
- Gerenciamento de recursos (moedas) para comprar torres e upgrades
- Jogo contínuo com dificuldade crescente

## Começando

### Pré-requisitos

- Unity 2020.3 LTS ou superior
- Git (para clonar o repositório)

### Instalação

1. Clone o repositório:
   ```
   git clone https://github.com/FreseEnzo/Seas-of-siege.git
   ```

2. Abra o Unity Hub

3. Clique em "Adicionar" e navegue até o diretório "Seas-of-siege" clonado

4. Selecione o projeto e clique em "Abrir"

5. Assim que o Unity carregar o projeto, navegue até a pasta "Scenes" na janela do Projeto

6. Clique duas vezes no arquivo de cena principal (por exemplo, "MainGame.unity") para abri-lo

7. Pressione o botão Play no Editor do Unity para executar o jogo

## Jogabilidade

### Geração de Grade

O jogo começa gerando um mapa baseado em grade que define:
- Pontos de spawn de inimigos
- Localização inicial da ilha
- Posição de início do personagem do jogador

### Geração de Terreno

Após a geração da grade, o jogo popula o mapa com vários ativos:
- Personagem do jogador
- Características do terreno (árvores, rochas, etc.)

### Sistema de Ondas

O jogo opera em um sistema baseado em ondas:
- Cada onda consiste em um determinado número de inimigos
- Derrotar todos os inimigos em uma onda permite que o jogador construa ou atualize
- As ondas se tornam progressivamente mais difíceis

### Criação de Inimigos

Durante as ondas:
- Os inimigos aparecem em pontos predeterminados
- Cada inimigo tem um ativo visual atribuído aleatoriamente
- Os inimigos miram no terreno mais próximo e se movem em linha reta
- Ao alcançar o terreno, os inimigos atacam e podem destruir o terreno

### Mecânicas de Jogo

- O personagem do jogador ataca automaticamente os inimigos próximos
- As torres atacam os inimigos dentro de seu alcance
- Inimigos derrotados deixam moedas para o jogador coletar

### Fase de Construção

Após derrotar uma onda, os jogadores entram na fase de construção:
- Construir novas torres
- Atualizar torres existentes
- Expandir a ilha

O jogador deve encerrar manualmente esta fase para iniciar a próxima onda.

### Tipos de Torres

1. **Mcc A**
   - Torre básica de ataque único
   - Dano e velocidade de ataque equilibrados

2. **Mcc B**
   - Dano de área de efeito
   - Dano e alcance menores em comparação com Mcc A

3. **Mcc C**
   - Alto dano, baixa velocidade de ataque
   - Maior alcance de todas as torres

Todas as torres podem ser atualizadas três vezes, melhorando o dano, a velocidade de ataque e o alcance.

### Condições de Fim de Jogo

O jogo termina quando a ilha atual do jogador é destruída enquanto ele ainda estiver nela.

## Desenvolvimento

Este projeto é desenvolvido usando Unity e C# com uma abordagem orientada a objetos. O principal loop de jogo segue estas etapas:

1. Geração de grade e terreno
2. Contagem de ondas e spawn de inimigos
3. Execução da jogabilidade (ataques do jogador e das torres, movimento de inimigos)
4. Fase de construção entre ondas
5. Loop contínuo até a derrota do jogador

## Licença

Este projeto está licenciado sob a [Licença MIT](https://github.com/FreseEnzo/Seas-of-siege/blob/main/LICENSE).