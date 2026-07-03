# Atividade 1.2 Avaliativa de 2026.1 - Sistemas Operacionais

Implementação de problemas clássicos de concorrência usando semáforos em C#.

## Problemas incluídos

- Produtor / Consumidor (`producer_consumer`)
- Leitores / Escritores (`readers_writers`)
- Jantar dos Filósofos (`dining_philosophers`)

## Como usar

### Construir o container

```bash
docker build -t so-concorrencia .
```

### Executar Produtor / Consumidor

```bash
docker run --rm so-concorrencia producer_consumer 2 2 10 5
```

Parâmetros: `num_producers num_consumers items_per_producer buffer_size`

### Executar Leitores / Escritores

```bash
docker run --rm so-concorrencia readers_writers 4 2 5
```

Parâmetros: `num_readers num_writers cycles`

### Executar Jantar dos Filósofos

```bash
docker run --rm so-concorrencia dining_philosophers 5 3
```

Parâmetros: `num_philosophers cycles`

## Observações

- O código usa `System.Threading.SemaphoreSlim` para sincronização.
- O projeto foi desenvolvido para rodar dentro de um container Linux com .NET.
- Registre as observações de execução e qualquer comportamento não esperado para a apresentação.


Link para apresentação: https://canva.link/sonc22jic5t0rzu
