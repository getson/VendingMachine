# VendingMachine

The solution is build using .NET Core 3.1

# Libraries used:
- FluentValidation - it is used for user input validations. I personally like it, and I use it in any project.
- Autofac - it is used for DI, I use it for years, i prefer it against Microsoft IoC for its advantages related to metadata registrations.
- CommandLineParser - it is used for parsing console arguments.
- MediatR - it is used to build CQRS, it is a simple library which helps a lot in CQRS

# Patterns
 - CQRS, 
 - State

For the sake of brevity, to manage the state of Vending Machine I have used State pattern.
When the the app is started I create an instance of VendingMachine and it will alter it's state according to user commands.

Vending Machine has 3 states:
 - ReadyToSellProduct - it means that the machine is in the state that only accept "select product request", after the user has selected a product,
                        the machine will go to the next state (ReadyToAcceptCoins)
 - ReadyToAcceptCoins - it means that the machine is in the state that can only accept "insert coins request".
                        from this state the machine go to ReadyToProcessOrder.
 - ReadyToProcessOrder- it means that the machine is in final state, when it will process the transaction.
                        In this state the machine is able to process requests "Process Order", "Cancel Order", after processing order it turns back the change
                        or all inserted coins.

Vending machine will accept all the requests through the terminal.
# Request categories 
 - a request that has side effects is interpreted as a "Command" (Select Product, Insert Coins, etc...)
 - a request for getting some info from the machine is interpreted as a "Query" (GetProductsWithPrices)
 
 Notifications for the user are managed through events.



# Supported features:
- Show the user the list of products and their price
- Select product to buy - the user can select a product, it will be notified when the product is not available.
- Insert coins          - after the user has selected the product it will be asked to insert the coins to buy it. 
                          when coins are not sufficient, the user will be notified and asked if he want to insert more coins in order 
                          to complete the buy process. If not, he can cancel the order, and take back his coins.
- Confirm/Cancel order  - The user has the possibility to confirm or cancel his order. In case that the machine is not able to give back the change,
                          it will notify the user, and ask him if he wants to confirm the order without taking back the change or to cancel order and take back his coins.
                          When the order is confirmed, the user will be notified to take the product and his change.

                          
 
 
