# Criando um Programa Simples no Unity 3D (Biblioteca Gráfica)

## Passo 1: Instalar o Unity e configurar o projeto

1. **Instalar o Unity Hub**:
   - Baixe o Unity Hub no site oficial do Unity [Unity Download](https://unity.com/download).
   - Siga as instruções de instalação e, ao abrir o Unity Hub, instale a versão mais recente do Unity.

2. **Criar um novo projeto**:
   - No Unity Hub, clique em "New Project".
   - Escolha o template “3D”.
   - Dê um nome ao projeto e selecione o local onde ele será salvo, depois clique em "Create Project".

## Passo 2: Configurar a cena no Unity

1. **Abrir a cena principal**:
   - Na aba “Hierarchy”, você verá uma cena já criada, chamada "SampleScene". Se não existir, crie uma nova cena clicando em `File -> New Scene`.

2. **Adicionar um objeto (Player)**:
   - No menu superior, clique em `GameObject -> 3D Object -> Cube`. Isso adicionará um cubo na cena.
   - Este será o nosso "Player". Para torná-lo mais identificável, renomeie o objeto clicando com o botão direito no cubo na aba "Hierarchy" e selecionando "Rename", chamando-o de "Player".

3. **Adicionar o terreno (Ground)**:
   - Crie outro objeto para ser o chão clicando em `GameObject -> 3D Object -> Plane`.
   - Renomeie o objeto para "Ground" e ajuste sua posição para (0, 0, 0) na aba "Inspector" (Position).

## Passo 3: Criar o script para movimentar o Player

1. **Adicionar o script de movimentação**:
   - Na aba "Assets", clique com o botão direito e selecione `Create -> C# Script`. Dê um nome ao script, como "PlayerMovement".
   - Dê um duplo clique no script criado para abri-lo no editor de código (provavelmente o Visual Studio).

2. **Escrever o código para movimentação**:
   O código a seguir vai capturar o input do teclado e mover o cubo (Player) pela cena.

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento

    void Update()
    {
        // Capturar inputs do teclado
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Criar o vetor de movimento
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Mover o jogador
        transform.Translate(movement * speed * Time.deltaTime);
    }
}
```

## Passo 4: Testar o projeto

1. **Adicionar a câmera:**
    Ajuste a câmera para que ela aponte para o player. Selecione a "Main Camera" na aba "Hierarchy" e ajuste sua posição no "Inspector" (por exemplo, posição X: 0, Y: 10, Z: -10, e rotação X: 45).

2. **Executar o projeto:**
        Clique no botão "Play" na parte superior da interface do Unity. Agora você deve conseguir mover o cubo pela cena usando as teclas direcionais ou as teclas W, A, S, D.

## Passo 5: Refinar o projeto

1. **Ajustar o terreno:**
        Se quiser melhorar a aparência do jogo, você pode adicionar materiais ao cubo e ao chão. Crie materiais em Assets -> Create -> Material e aplique texturas.

2. **Adicionar física:**
        Para melhorar a jogabilidade, você pode adicionar componentes de física como Rigidbody no Player. Selecione o Player, clique em "Add Component" e adicione "Rigidbody" para que o objeto tenha simulação de física.

## Conclusão

Este é um exemplo simples de como criar um projeto básico no Unity. Ele inclui um cenário básico e a movimentação de um objeto 3D controlado pelo teclado. A partir disso, você pode continuar aprimorando o projeto, adicionando mais funcionalidades como colisões, efeitos visuais, sons e muito mais.