using System;

// ============================================================
// PARTE 1 - TAD Pilha com vetor
// ============================================================
public class Pilha
{
    private int[] item;   // vetor que guarda os elementos
    private int topo;     // próxima posição livre = quantidade de elementos

    public Pilha(int maxTam)
    {
        item = new int[maxTam];
        topo = 0;                       // pilha começa vazia
    }

    public bool vazia()   { return topo == 0; }
    public bool cheia()   { return topo == item.Length; }
    public int tamanho()  { return topo; }

    public void empilha(int x)
    {
        if (cheia())
            throw new Exception("Erro: pilha cheia");
        item[topo] = x;                 // escreve na próxima posição livre
        topo++;                         // e avança o topo
    }

    public int desempilha()
    {
        if (vazia())
            throw new Exception("Erro: pilha vazia");
        topo--;                         // só recua o topo...
        return item[topo];              // ...o valor fica no vetor (lixo)
    }

    public int consultaTopo()
    {
        if (vazia())
            throw new Exception("Erro: pilha vazia");
        return item[topo - 1];          // olha, mas NÃO remove
    }

    public void mostra()                // base -> topo, ex.: [1, 3, 5]
    {
        Console.Write("[");
        for (int i = 0; i < topo; i++)
        {
            Console.Write(item[i]);
            if (i < topo - 1) Console.Write(", ");
        }
        Console.WriteLine("]");
    }
}

public class Program
{
    // ========================================================
    // PARTE 3.1 - Inverter texto
    // ========================================================
    static string inverte(string s)
    {
        Pilha p = new Pilha(s.Length);
        foreach (char c in s)
            p.empilha(c);                       // char -> int automático

        string resultado = "";
        while (!p.vazia())
            resultado += (char)p.desempilha();  // int -> char (cast explícito)
        return resultado;
    }

    // ========================================================
    // PARTE 3.2 - Parênteses balanceados
    // ========================================================
    static bool balanceado(string expr)
    {
        Pilha p = new Pilha(expr.Length);

        foreach (char c in expr)
        {
            if (c == '(' || c == '[' || c == '{')
            {
                p.empilha(c);                   // abertura: guarda
            }
            else if (c == ')' || c == ']' || c == '}')
            {
                if (p.vazia())                  // fechou sem ter aberto
                    return false;

                char abertura = (char)p.desempilha();
                bool par = (abertura == '(' && c == ')') ||
                           (abertura == '[' && c == ']') ||
                           (abertura == '{' && c == '}');
                if (!par)                       // tipo errado / ordem errada
                    return false;
            }
            // qualquer outro caractere (a, +, *, ...) é ignorado
        }

        return p.vazia();                       // sobrou abertura? não balanceado
    }

    // ========================================================
    // PARTE 4.2 - Bubble Sort com três pilhas (A, B, R) + carry
    // ========================================================
    static int[] bubblePilhas(int[] v, out int comparacoes)
    {
        int n = v.Length;
        comparacoes = 0;

        Pilha A = new Pilha(n);
        Pilha B = new Pilha(n);
        Pilha R = new Pilha(n);

        // carrega o vetor em A, na ordem do vetor (último fica no topo)
        for (int i = 0; i < n; i++)
            A.empilha(v[i]);

        for (int restantes = n; restantes >= 1; restantes--)
        {
            int carry = A.desempilha();                 // a "bolha"

            for (int k = 0; k < restantes - 1; k++)
            {
                int x = A.desempilha();
                comparacoes++;
                if (carry > x)
                {
                    B.empilha(x);                       // bolha segue com carry
                }
                else
                {
                    B.empilha(carry);                   // x vira a nova bolha
                    carry = x;
                }
            }

            R.empilha(carry);                           // maior dos restantes -> R

            Pilha tmp = A; A = B; B = tmp;              // troca os papéis de A e B
        }

        // R tem o maior na base e o menor no topo:
        // desempilhar R devolve em ordem crescente
        int[] ordenado = new int[n];
        for (int i = 0; i < n; i++)
            ordenado[i] = R.desempilha();
        return ordenado;
    }

    // ========================================================
    // DESAFIO - Ordenar uma pilha (maior valor no topo)
    // ========================================================
    static Pilha ordenaPilha(Pilha p)
    {
        // tamanho() é uma operação do TAD; no início é a quantidade total
        Pilha aux = new Pilha(p.tamanho());

        while (!p.vazia())
        {
            int t = p.desempilha();

            // devolve para a entrada tudo que na auxiliar é maior que t
            while (!aux.vazia() && aux.consultaTopo() > t)
                p.empilha(aux.desempilha());

            aux.empilha(t);     // t cai na posição certa (aux fica crescente base->topo)
        }
        return aux;             // maior valor no topo
    }

    // ========================================================
    // Auxiliar de impressão de vetores
    // ========================================================
    static void imprimeVetor(int[] v)
    {
        Console.WriteLine("[" + string.Join(", ", v) + "]");
    }

    // ========================================================
    // MAIN - testes de todas as partes
    // ========================================================
    public static void Main(string[] args)
    {
        // ---------- Parte 1: teste do enunciado ----------
        Console.WriteLine("=== PARTE 1 ===");
        Pilha p = new Pilha(5);
        p.empilha(1); p.empilha(3); p.empilha(5); p.empilha(7);
        p.mostra();                                   // [1, 3, 5, 7]
        Console.WriteLine(p.desempilha());            // 7
        Console.WriteLine(p.consultaTopo());          // 5
        p.empilha(8); p.empilha(9);
        p.mostra();                                   // [1, 3, 5, 8, 9]
        Console.WriteLine(p.cheia());                 // True
        try { p.empilha(2); }
        catch (Exception e) { Console.WriteLine(e.Message); }   // Erro: pilha cheia

        // ---------- Parte 2: conferir o rastreio ----------
        Console.WriteLine("\n=== PARTE 2 (rastreio) ===");
        Pilha q = new Pilha(4);
        q.empilha(10); q.mostra();
        q.empilha(20); q.mostra();
        Console.WriteLine(q.desempilha()); q.mostra();
        q.empilha(30); q.mostra();
        q.empilha(40); q.mostra();
        Console.WriteLine(q.desempilha()); q.mostra();
        Console.WriteLine(q.desempilha()); q.mostra();
        q.empilha(50); q.mostra();
        Console.WriteLine("tamanho = " + q.tamanho());  // 2

        // ---------- Parte 3.1 ----------
        Console.WriteLine("\n=== PARTE 3.1 ===");
        Console.WriteLine(inverte("PUCMINAS"));        // SANIMCUP

        // ---------- Parte 3.2 ----------
        Console.WriteLine("\n=== PARTE 3.2 ===");
        string[] testes = { "(a+b)*[c-d]", "{[()]}", "((a)", "a+b)", "([)]" };
        foreach (string t in testes)
            Console.WriteLine(t + " -> " + balanceado(t).ToString().ToLower());

        // ---------- Parte 4.2 ----------
        Console.WriteLine("\n=== PARTE 4.2 ===");
        int comp;
        int[] r1 = bubblePilhas(new int[] { 5, 2, 9, 1, 7 }, out comp);
        imprimeVetor(r1);
        Console.WriteLine("comparacoes = " + comp);     // 10

        int[] r2 = bubblePilhas(new int[] { 1, 2, 3, 4, 5 }, out comp);
        imprimeVetor(r2);
        Console.WriteLine("comparacoes (ja ordenado) = " + comp);   // 10

        // ---------- Desafio ----------
        Console.WriteLine("\n=== DESAFIO ===");
        Pilha d = new Pilha(5);
        d.empilha(5); d.empilha(2); d.empilha(9); d.empilha(1); d.empilha(7);
        Pilha ord = ordenaPilha(d);
        ord.mostra();                                   // [1, 2, 5, 7, 9]
    }
}