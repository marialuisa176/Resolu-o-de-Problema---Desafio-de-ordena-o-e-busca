using System;

class Program
{
    // Dicionários para classes e subclasses
    static string[] Classes = { "Alimento", "Limpeza", "Higiene" };
    static string[] Subclasses = { "Carne", "Biscoito", "Frutas/Legumes", "Sabão em pó", "Detergente", "Amaciante", "Shampoo", "Condicionador", "Sabonete" };

    // Função para mostrar os produtos
    static void MostrarProdutos(int[,] produtos, double[] precos)
    {
        Console.WriteLine("\nProdutos Classificados:");
        for (int i = 0; i < produtos.GetLength(0); i++)
        {
            int classe = produtos[i, 0] / 10;   // Primeira parte do código, indicando a classe
            int subclasse = produtos[i, 0] % 10; // Segunda parte do código, indicando a subclasse
            string nomeProduto = (produtos[i, 1] == 1) ? "Produto A" : "Produto B"; // Nome fictício de produtos
            double precoProduto = precos[i]; // Acesso ao preço do produto (agora em uma matriz separada)

            Console.WriteLine($"{produtos[i, 0]} → {Classes[classe - 1]} > {Subclasses[subclasse - 1]} > {nomeProduto} - Preço: R${precoProduto:F2}");
        }
    }

    // Função principal
    static void Main()
    {
        // Pergunta ao usuário quantos produtos ele deseja inserir
        Console.Write("Quantos produtos você deseja cadastrar? ");
        int n;
        while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
        {
            Console.Write("Por favor, insira um número válido de produtos: ");
        }

        // Definir a matriz de produtos e um array separado para os preços
        int[,] produtos = new int[n, 2];  // Matriz de produtos com 2 colunas: Código, Nome (1 ou 2)
        double[] precos = new double[n];  // Array para armazenar o preço dos produtos (tipo double)

        // Solicita dados do produto para o usuário
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"\nProduto {i + 1}:");

            // Código do produto (classe + subclasse)
            int codigoProduto;
            while (true)
            {
                Console.Write("Digite o código do produto (classe + subclasse, por exemplo 12 para Alimento > Biscoito): ");
                if (int.TryParse(Console.ReadLine(), out codigoProduto) && codigoProduto >= 10 && codigoProduto <= 39)
                {
                    produtos[i, 0] = codigoProduto;
                    break;
                }
                else
                {
                    Console.WriteLine("Código inválido. Por favor, insira um código no formato XX (exemplo: 12, 13, etc.).");
                }
            }

            // Nome do produto (1 para Produto A, 2 para Produto B)
            int nomeProduto;
            while (true)
            {
                Console.Write("Digite o nome do produto (1 para Produto A, 2 para Produto B): ");
                if (int.TryParse(Console.ReadLine(), out nomeProduto) && (nomeProduto == 1 || nomeProduto == 2))
                {
                    produtos[i, 1] = nomeProduto;
                    break;
                }
                else
                {
                    Console.WriteLine("Nome inválido. Digite 1 para Produto A ou 2 para Produto B.");
                }
            }

            // Preço do produto
            double precoProduto;
            while (true)
            {
                Console.Write("Digite o preço do produto: ");
                if (double.TryParse(Console.ReadLine(), out precoProduto) && precoProduto >= 0)
                {
                    precos[i] = precoProduto;  // Armazenando o preço na array separada de preços
                    break;
                }
                else
                {
                    Console.WriteLine("Preço inválido. Insira um preço válido (exemplo: 10.50).");
                }
            }
        }

        Console.WriteLine("\nProdutos antes da classificação:");
        MostrarProdutos(produtos, precos);

        // Classificando os produtos com Radix Sort
        RadixSort(produtos);

        // Mostrando os produtos classificados
        MostrarProdutos(produtos, precos);
    }

    // Radix Sort modificado para trabalhar com matrizes
    public static void RadixSort(int[,] produtos)
    {
        int max = produtos[0, 0];
        for (int i = 1; i < produtos.GetLength(0); i++)
            if (produtos[i, 0] > max) max = produtos[i, 0];

        for (int exp = 1; max / exp > 0; exp *= 10)
            CountSort(produtos, exp);
    }

    // Função auxiliar de Counting Sort para matrizes
    private static void CountSort(int[,] produtos, int exp)
    {
        int[,] output = new int[produtos.GetLength(0), 2]; // Modificado para armazenar 2 colunas: código e nome
        int[] count = new int[10];

        // Contagem dos elementos
        for (int i = 0; i < produtos.GetLength(0); i++)
            count[(produtos[i, 0] / exp) % 10]++;

        // Cálculo da posição
        for (int i = 1; i < 10; i++)
            count[i] += count[i - 1];

        // Ordenação baseada no dígito
        for (int i = produtos.GetLength(0) - 1; i >= 0; i--)
        {
            int num = produtos[i, 0];
            int index = count[(num / exp) % 10] - 1;
            output[index, 0] = produtos[i, 0];
            output[index, 1] = produtos[i, 1];
            count[(num / exp) % 10]--;
        }

        // Copiar o array de saída de volta para o array original
        for (int i = 0; i < produtos.GetLength(0); i++)
            for (int j = 0; j < 2; j++)  // A matriz agora tem apenas 2 colunas
                produtos[i, j] = output[i, j];
    }
}